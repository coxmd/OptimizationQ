let socket;

function startWebSocket(dotNetHelper) {
    // Store helper for later
    window.dotNetHelper = dotNetHelper;

    // Open a WebSocket connection
    socket = new WebSocket("wss://localhost:7055/api/fingerprint/ws");

    // WebSocket connection opened
    socket.onopen = () => {
        console.log("WebSocket connection opened");
    };

    // Receive message from WebSocket
    socket.onmessage = async (event) => {
        const jsonData = JSON.parse(event.data);

        if (jsonData.fingerprintData) {
            console.log("Received fingerprint data");
            await window.dotNetHelper.invokeMethodAsync('ReceiveFingerprintData', jsonData.fingerprintData);
        }
    };

    // Handle errors
    socket.onerror = (error) => {
        console.error('WebSocket error:', error);
    };

    // Cleanup when WebSocket closes
    window.addEventListener('unload', () => {
        if (socket) {
            socket.close();
        }
    });
}





//function startFingerprintPolling(dotNetHelper) {
//    // Store the dotNetHelper reference
//    window.dotNetHelper = dotNetHelper;
//    // Poll every 2 seconds (2000 milliseconds)
//    setInterval(fetchFingerprintData, 2000);
//}
//async function fetchFingerprintData() {
//    try {
//        const response = await fetch('https://localhost:7055/api/fingerprint/Authenticate');
//        if (response.ok) {

//            // Call the .NET method using the stored helper
//            const jsonData = await response.json();
//            const base64String = jsonData.fingerprintData;
//                await window.dotNetHelper.invokeMethodAsync('ReceiveFingerprintData', base64String);
//            //}
//        }
//    } catch (error) {
//        console.error('Error fetching fingerprint data:', error);
//    }
//}

//// Helper function to convert ArrayBuffer to base64
//function arrayBufferToBase64(buffer) {
//    const binary = String.fromCharCode.apply(null, new Uint8Array(buffer));
//    return window.btoa(binary);

//}

//function startFingerprintPolling(dotNetHelper) {
//    let lastFingerprint = null;
//    let pollingInterval = null;
    
//    // Store helper with cleanup method
//    window.dotNetHelper = {
//        helper: dotNetHelper,
//        cleanup: () => {
//            if (pollingInterval) {
//                clearInterval(pollingInterval);
//            }
//            window.dotNetHelper = null;
//        }
//    };

//    // Implement exponential backoff
//    let retryDelay = 2000;
//    const maxDelay = 30000; // Max 30 seconds

//    async function fetchWithBackoff() {
//        try {
//            const response = await fetch('https://localhost:7055/api/fingerprint/Authenticate');
//            if (response.ok) {
//                const jsonData = await response.json();
                
//                // Only invoke if data changed
//                if (lastFingerprint !== jsonData.fingerprintData) {
//                    lastFingerprint = jsonData.fingerprintData;
//                    await window.dotNetHelper.helper.invokeMethodAsync('ReceiveFingerprintData', jsonData.fingerprintData);
//                }
                
//                // Reset delay on success
//                retryDelay = 2000;
//            }
//        } catch (error) {
//            console.error('Error fetching fingerprint data:', error);
            
//            // Implement backoff on error
//            retryDelay = Math.min(retryDelay * 1.5, maxDelay);
//        }
//    }

//    // Start polling with dynamic interval
//    pollingInterval = setInterval(fetchWithBackoff, retryDelay);

//    // Add cleanup on page unload
//    window.addEventListener('unload', window.dotNetHelper.cleanup);
//}


