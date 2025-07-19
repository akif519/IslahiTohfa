// Main JavaScript for Islahi Tohfa
window.blazorCulture = {
    get: () => window.localStorage['BlazorCulture'],
    set: (value) => window.localStorage['BlazorCulture'] = value
};

// Utility functions
window.downloadFile = function (url, filename) {
    const a = document.createElement('a');
    a.href = url;
    a.download = filename;
    document.body.appendChild(a);
    a.click();
    document.body.removeChild(a);
};

window.copyToClipboard = function (text) {
    navigator.clipboard.writeText(text);
};

// PDF Viewer functions
window.initializePDFViewer = function (url) {
    // PDF.js initialization code will go here
    console.log('Initializing PDF viewer for:', url);
};

// Islamic features
window.getPrayerTime = function () {
    // Prayer time calculation
    const now = new Date();
    return now.toLocaleString('ar-SA');
};

// Theme toggle
window.toggleTheme = function () {
    document.body.classList.toggle('dark-theme');
};