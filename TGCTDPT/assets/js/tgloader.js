
(function () {

    const loader = document.getElementById('tgLoader');
    const mainContent = document.getElementById('mainContent');
    const loadingText = document.getElementById('tgLoadingText');
    const fetchButton = document.getElementById('fetchDataBtn');
    const resultArea = document.getElementById('resultArea');

    let currentPercent = 0;
    let percentInterval = null;
    let messageInterval = null;
    let isLoaderActive = false;

    const messages = [
        "Loading....."
    ];

    function updateLoadingText() {
        if (!loadingText) return;
        if (currentPercent >= 100) {
            loadingText.textContent = "Complete!";
            return;
        }
        const idx = Math.floor((currentPercent / 100) * messages.length);
        const safeIdx = Math.min(idx, messages.length - 1);
        loadingText.textContent = messages[safeIdx];
    }

    function resetLoaderState() {
        if (percentInterval) {
            clearInterval(percentInterval);
            percentInterval = null;
        }
        if (messageInterval) {
            clearInterval(messageInterval);
            messageInterval = null;
        }
        currentPercent = 0;
        isLoaderActive = false;
        if (loadingText) {
            loadingText.textContent = "Loading Tax Portal";
        }
    }

    function TGshowLoader() {
        resetLoaderState();

        loader.classList.remove('hidden');
        isLoaderActive = true;
        currentPercent = 0;
        updateLoadingText();

        percentInterval = setInterval(() => {
            if (isLoaderActive && currentPercent < 95) {
                const increment = Math.floor(Math.random() * 3) + 1;
                currentPercent = Math.min(currentPercent + increment, 95);
                updateLoadingText();
            }
        }, 120);

        messageInterval = setInterval(() => {
            if (isLoaderActive && currentPercent < 95) {
                updateLoadingText();
            }
        }, 1000);
    }

    function TGhideLoader() {
        isLoaderActive = false;

        if (percentInterval) {
            clearInterval(percentInterval);
            percentInterval = null;
        }

        currentPercent = 100;
        updateLoadingText();

        setTimeout(() => {
            loader.classList.add('hidden');
            resetLoaderState();
        }, 500);
    }


})();