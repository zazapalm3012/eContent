// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

// Add a global error handler for AJAX requests
$(document).ajaxError(function (event, xhr, settings, thrownError) {
    if (xhr.status === 0) {
        alert('Network error or server is unreachable.');
    } else if (xhr.status === 404) {
        alert('The requested resource was not found.');
    } else if (xhr.status === 500) {
        alert('Internal server error.');
    } else {
        alert(`An unexpected error occurred: ${thrownError || xhr.statusText}`);
    }
});

// Add a global error handler for uncaught exceptions
window.onerror = function (message, source, lineno, colno, error) {
    console.error("Uncaught error:", message, source, lineno, colno, error);
    alert(`An unexpected client-side error occurred: ${message}`);
    return true; // Prevent default error handling
};

// Add a global unhandledrejection handler for unhandled promise rejections
window.addEventListener('unhandledrejection', function (event) {
    console.error("Unhandled promise rejection:", event.reason);
    alert(`An unhandled promise rejection occurred: ${event.reason}`);
});