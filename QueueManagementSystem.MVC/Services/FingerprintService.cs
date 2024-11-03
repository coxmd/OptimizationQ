using System;
using System.Threading.Tasks;
using libzkfpcsharp;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using System.Drawing;
using System.Security.Cryptography;
using QueueManagementSystem.MVC.Data;
using Microsoft.EntityFrameworkCore;
using QueueManagementSystem.MVC.Models;
using FastReport.AdvMatrix;
using static QueueManagementSystem.MVC.Components.TicketTrackingPage;

namespace QueueManagementSystem.MVC.Services
{
    public class FingerprintService : IFingerprintService, IAsyncDisposable
    {
        private readonly ILogger<FingerprintService> _logger;
        private IntPtr mDevHandle = IntPtr.Zero;
        private IntPtr mDBHandle = IntPtr.Zero;
        private byte[] FPBuffer;
        private int RegisterCount = 0;
        private const int REGISTER_FINGER_COUNT = 3;
        private byte[][] RegTmps = new byte[3][];
        private byte[] RegTmp = new byte[2048];
        private byte[] CapTmp = new byte[2048];
        private int cbCapTmp = 2048;
        private int cbRegTmp = 0;
        private int iFid = 1;
        private int mfpWidth = 0;
        private int mfpHeight = 0;
        private readonly IDbContextFactory<QueueManagementSystemContext> _dbFactory;

        public FingerprintService(ILogger<FingerprintService> logger, IDbContextFactory<QueueManagementSystemContext> dbFactory)
        {
            _dbFactory = dbFactory;
            _logger = logger;
        }

        public async Task<bool> InitializeAsync()
        {
            return await Task.Run(() =>
            {
                int ret = zkfp2.Init();
                if (ret == zkfperrdef.ZKFP_ERR_OK)
                {
                    _logger.LogInformation("ZKFinger SDK initialized successfully");
                    // Device count
                    //int count = zkfp2.GetDeviceCount();
                    //_logger.LogInformation($"Number of connected devices: {count}");
                    //if (count == 0)
                    //{
                    //    _logger.LogError("No fingerprint devices found");
                    //    return false;
                    //}

                    //mDevHandle = zkfp2.OpenDevice(count - 1);
                    //if (mDevHandle == IntPtr.Zero)
                    //{
                    //    _logger.LogError("OpenDevice failed");
                    //    return false;
                    //}

                    mDBHandle = zkfp2.DBInit();
                    if (mDBHandle == IntPtr.Zero)
                    {
                        _logger.LogError("Failed to initialize fingerprint database");
                        zkfp2.CloseDevice(mDevHandle);
                        mDevHandle = IntPtr.Zero;
                        return false;
                    }

                    for (int i = 0; i < REGISTER_FINGER_COUNT; i++)
                    {
                        RegTmps[i] = new byte[2048];
                    }

                    byte[] paramValue = new byte[4];
                    int size = 4;
                    zkfp2.GetParameters(mDevHandle, 1, paramValue, ref size);
                    zkfp2.ByteArray2Int(paramValue, ref mfpWidth);

                    size = 4;
                    zkfp2.GetParameters(mDevHandle, 2, paramValue, ref size);
                    zkfp2.ByteArray2Int(paramValue, ref mfpHeight);

                    FPBuffer = new byte[mfpWidth * mfpHeight];
                    _logger.LogInformation("Fingerprint database initialized successfully");
                    _logger.LogInformation("Fingerprint device opened successfully");

                    return true;
                }
                else
                {
                    _logger.LogError($"Failed to initialize ZKFinger SDK. Error code: {ret}");
                    return false;
                }
            }).ConfigureAwait(false);
        }

        public async Task<int> GetDeviceCountAsync()
        {
            return await Task.Run(() =>
            {
                int count = zkfp2.GetDeviceCount();
                _logger.LogInformation($"Number of connected devices: {count}");
                return count;
            }).ConfigureAwait(false);
        }

        public async Task<bool> OpenDeviceAsync()
        {
            return await Task.Run(() =>
            {
                int deviceCount = zkfp2.GetDeviceCount();
                if (deviceCount == 0)
                {
                    _logger.LogWarning("No fingerprint devices found");
                    return false;
                }

                mDevHandle = zkfp2.OpenDevice(0);
                if (mDevHandle == IntPtr.Zero)
                {
                    _logger.LogError("Failed to open the fingerprint device");
                    return false;
                }

                mDBHandle = zkfp2.DBInit();
                if (mDBHandle == IntPtr.Zero)
                {
                    _logger.LogError("Failed to initialize the fingerprint database");
                    zkfp2.CloseDevice(mDevHandle);
                    mDevHandle = IntPtr.Zero;
                    return false;
                }

                _logger.LogInformation("Fingerprint device opened successfully");
                return true;
            }).ConfigureAwait(false);
        }

        public async Task<FingerprintResult> AuthenticateAsync()
        {
            return await Task.Run(() =>
            {
                byte[] fingerPrintData = new byte[2048];
                using var context = _dbFactory.CreateDbContext();
                var fullRecord = context.Biodata.FirstOrDefault(b => b.IdNumber == "30321983");

                if (fullRecord != null)
                {
                    fingerPrintData = fullRecord.FingerPrintData!;
                }
                else
                {
                    // Enrollment is required
                    var enrollResult = EnrollAsync().Result;
                    if (!enrollResult.IsAuthenticated)
                    {
                        return new FingerprintResult { IsAuthenticated = false, Message = enrollResult.Message };
                    }
                    fullRecord = context.Biodata.FirstOrDefault(b => b.IdNumber == "30321983");
                    if (fullRecord != null)
                    {
                        fingerPrintData = fullRecord.FingerPrintData!;
                    }
                    else
                    {
                        return new FingerprintResult { IsAuthenticated = false, Message = "Enrollment failed" };
                    }
                }

                if (CaptureFingerprint())
                {
                    int ret = zkfp2.DBMatch(mDBHandle, CapTmp, fingerPrintData);
                    if (ret > 0)
                    {
                        _logger.LogInformation($"Match finger succeeded, score={ret}!");

                        //Array.Clear(CapTmp, 0, CapTmp.Length); // Clear buffer for next capture
                        return new FingerprintResult
                        {
                            IsAuthenticated = true,
                            Message = $"Match finger succeeded, score={ret}!",
                            // Optionally populate Customer details here
                        };
                    }
                    else
                    {
                        _logger.LogError($"Match finger failed, ret={ret}");
                        //Array.Clear(CapTmp, 0, CapTmp.Length); // Clear buffer for next capture
                        return new FingerprintResult
                        {
                            IsAuthenticated = false,
                            Message = $"Match finger failed, ret={ret}"
                        };
                    }
                }
                else
                {
                    return new FingerprintResult
                    {
                        IsAuthenticated = false,
                        Message = "Device Not Ready"
                    };
                }
            }).ConfigureAwait(false);
        }
        public async Task<FingerprintResult> SearchIdnoAsync(Models.CustomerInfo customer)
        {
            return await Task.Run(() =>
            {
                // Check if fingerprint capture is successful
                if (CaptureFingerprint())
                {
                    using var context = _dbFactory.CreateDbContext();
                    var customerInfo = context.Biodata.FirstOrDefault(b => b.IdNumber == customer.Idnumber);

                    if (customerInfo != null)
                    {
                        byte[] fingerPrintData = customerInfo.FingerPrintData!;
                        int attempts = 3;

                        // Attempt fingerprint matching with retry logic
                        while (attempts > 0)
                        {
                            int ret = zkfp2.DBMatch(mDBHandle, CapTmp, fingerPrintData);

                            if (ret > 0) // Successful match
                            {
                                _logger.LogInformation($"Fingerprint match succeeded, score={ret}!");

                                return new FingerprintResult
                                {
                                    IsAuthenticated = true,
                                    Message = $"Fingerprint match succeeded, score={ret}!",
                                    // Optionally, populate Customer details here
                                };
                            }
                            else // Failed match
                            {
                                _logger.LogError($"Fingerprint match failed, ret={ret}");
                                attempts--;
                                return new FingerprintResult
                                {
                                    IsAuthenticated = false,

                                    Message = "Fingerprint match failed for attemp " + attempts,
                                    
                                };
                            }
                        }

                        // If no successful match after attempts
                        return new FingerprintResult
                        {
                            IsAuthenticated = true,
                            Message = "Fingerprint match failed after 3 attempts."
                        };
                    }
                    else
                    {
                        // Customer not found, enroll fingerprint or handle case
                        
                        Biometrics biodata = new Biometrics
                        {
                            IdNumber = customer.Idnumber,
                            FingerPrintData = CapTmp
                        };
                        context.Add(biodata);
                        context.SaveChanges();
                        _logger.LogInformation("Enroll success");
                        return new FingerprintResult
                        {
                            IsAuthenticated = true,
                            Message = "Customer not found.Fingerprint regsitered Successfully."
                        };
                    }
                }
                else
                {
                    // Device not ready or capture failed
                    return new FingerprintResult
                    {
                        IsAuthenticated = false,
                        Message = "Device not ready"
                    };

                }
            }).ConfigureAwait(false);
        }

        public async Task<FingerprintResult> EnrollAsync()
        {
            return await Task.Run(() =>
            {
                if (CaptureFingerprint())
                {
                    int ret = zkfp.ZKFP_ERR_OK;

                    Array.Copy(CapTmp, RegTmps[RegisterCount], cbCapTmp);
                    RegisterCount++;

                    if (RegisterCount >= REGISTER_FINGER_COUNT)
                    {
                        RegisterCount = 0;
                        ret = zkfp2.DBMerge(mDBHandle, RegTmps[0], RegTmps[1], RegTmps[2], RegTmp, ref cbRegTmp);
                        if (ret == zkfp.ZKFP_ERR_OK)
                        {
                            ret = zkfp2.DBAdd(mDBHandle, iFid, RegTmp);
                            if (ret == zkfp.ZKFP_ERR_OK)
                            {
                                iFid++;
                                using var context = _dbFactory.CreateDbContext();
                                Biometrics biodata = new Biometrics
                                {
                                    IdNumber = "30321983",
                                    FingerPrintData = RegTmp
                                };
                                context.Add(biodata);
                                context.SaveChanges();
                                _logger.LogInformation("Enroll success");
                                return new FingerprintResult
                                {
                                    IsAuthenticated = true,
                                    Message = "Finger enrollment was successful"
                                };
                            }
                        }

                        _logger.LogError($"Enroll failed, error code={ret}");
                        return new FingerprintResult
                        {
                            IsAuthenticated = false,
                            Message = $"Enroll failed, error code={ret}"
                        };
                    }
                    else
                    {
                        _logger.LogInformation($"You need to press the fingerprint {REGISTER_FINGER_COUNT - RegisterCount} more time(s)");
                        return new FingerprintResult
                        {
                            IsAuthenticated = false,
                            Message = $"You need to press the fingerprint {REGISTER_FINGER_COUNT - RegisterCount} more time(s)"
                        };
                    }
                }

                return new FingerprintResult
                {
                    IsAuthenticated = false,
                    Message = "Device not ready, lift your finger and press again"
                };
            }).ConfigureAwait(false);
        }

        public async Task CloseDeviceAsync()
        {
            await Task.Run(() =>
            {
                if (mDevHandle != IntPtr.Zero)
                {
                    zkfp2.CloseDevice(mDevHandle);
                    mDevHandle = IntPtr.Zero;
                    _logger.LogInformation("Fingerprint device closed");
                }
            }).ConfigureAwait(false);
        }

        private bool CaptureFingerprint()
        {
            cbCapTmp = 2048;
            FPBuffer = new byte[mfpWidth * mfpHeight];
            int ret = zkfp2.AcquireFingerprint(mDevHandle, FPBuffer, CapTmp, ref cbCapTmp);
            if (ret == zkfp.ZKFP_ERR_OK)
            {
                _logger.LogInformation("Fingerprint acquired successfully");
                return true;
               
            }
            else
            {
                _logger.LogWarning($"Failed to acquire fingerprint, ret={ret}");
                return false;
            }
        }

        public async ValueTask DisposeAsync()
        {
            await CloseDeviceAsync().ConfigureAwait(false);
            zkfp2.Terminate();
            _logger.LogInformation("Fingerprint service disposed");
        }

        public async Task<FingerprintResult> ContinuousScanAsync(Models.CustomerInfo customer)
        {
            return await Task.Run(() =>
            {
                if (!CaptureFingerprint())
                {
                    _logger.LogWarning("Failed to acquire fingerprint");
                    return new FingerprintResult
                    {
                        IsAuthenticated = false,
                        Message = "No fingerprint detected"
                    };
                }

                using var context = _dbFactory.CreateDbContext();
                var customerInfo = context.Biodata.FirstOrDefault(b => b.IdNumber == customer.Idnumber);

                if (customerInfo != null && customerInfo.FingerPrintData != null)
                {
                    int ret = zkfp2.DBMatch(mDBHandle, CapTmp, customerInfo.FingerPrintData);
                    if (ret > 0)
                    {
                        _logger.LogInformation($"Fingerprint match succeeded, score={ret}!");
                        return new FingerprintResult
                        {
                            IsAuthenticated = true,
                            Message = $"Fingerprint match succeeded, score={ret}!"
                        };
                    }
                    else
                    {
                        _logger.LogWarning($"Fingerprint match failed, score={ret}");
                        return new FingerprintResult
                        {
                            IsAuthenticated = false,
                            Message = "Fingerprint match failed"
                        };
                    }
                }
                else
                {
                    // Customer not found or no fingerprint data, enroll fingerprint
                    Biometrics biodata = new Biometrics
                    {
                        IdNumber = customer.Idnumber,
                        FingerPrintData = CapTmp
                    };
                    context.Add(biodata);
                    context.SaveChanges();
                    _logger.LogInformation("Fingerprint registered successfully");
                    return new FingerprintResult
                    {
                        IsAuthenticated = true,
                        Message = "Fingerprint registered successfully."
                    };
                }
            });
        }

        public async Task<FingerprintResult> MatchFingerPrintAsync(Models.CustomerInfo customer)
        {
            return await Task.Run(() =>
            {

                using var context = _dbFactory.CreateDbContext();
                // Fetch the list of fingerprints from the database
                var fingerprints = context.Biodata.Where(b => b.FingerPrintData != null).ToList();
                var fingerPrintCount= fingerprints.Count();
               
                    foreach (var fingerprint in fingerprints)
                {
                    // Check for a matching customer info based on fingerprint data
                    int ret = zkfp2.DBMatch(mDBHandle, customer.FingerPrintData, fingerprint.FingerPrintData);
                    _logger.LogInformation($"score={ret}!");
                    if (ret > 0)
                        {
                            _logger.LogInformation($"Fingerprint match succeeded, score={ret}!");
                            return new FingerprintResult
                            {
                                IsAuthenticated = true,
                                CustomerIdno = fingerprint.IdNumber!,
                                Message = $"Fingerprint match succeeded, score={ret}!"
                            };
                        }

                        
                    }

                


                return new FingerprintResult
                {
                    IsAuthenticated = false,
                    
                    Message = $"No Registered Fingerprint data"
                };

            });
        }

    }

    public class FingerprintResult
    {
        public bool IsAuthenticated { get; set; }
        public string Message { get; set; }
        public bool IsSuccess { get; set; }
        public string CustomerName { get; set; }
        public string CustomerIdno { get; set; }
        public string CustomerPhoneNumber { get; set; }
        public string CustomerEmail { get; set; }
        public byte[]? FingerPrintData { get; set; }
    }
}
