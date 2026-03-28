// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function togglePasswordVisibility() {
    var passwordInput = document.getElementById("password");
    var eyeIcon = document.getElementById("eyeIcon");
    if (passwordInput.type === "password") {
        passwordInput.type = "text";
        eyeIcon.src = "/theme/images/eyeopen.svg";
    } else {
        passwordInput.type = "password";
        eyeIcon.src = "/theme/images/eyeclose.svg";
    }
}

function toggleOldPasswordVisibility() {
    var passwordInput = document.getElementById("oldpassword");
    var eyeIcon = document.getElementById("eyeIcon");
    if (passwordInput.type === "password") {
        passwordInput.type = "text";
        eyeIcon.src = "/theme/images/eyeopen.svg";
    } else {
        passwordInput.type = "password";
        eyeIcon.src = "/theme/images/eyeclose.svg";
    }
}

function toggleNewPasswordVisibility() {
    var passwordInput = document.getElementById("newpassword");
    var eyeIcon = document.getElementById("eyeIcon");
    if (passwordInput.type === "password") {
        passwordInput.type = "text";
        eyeIcon.src = "/theme/images/eyeopen.svg";
    } else {
        passwordInput.type = "password";
        eyeIcon.src = "/theme/images/eyeclose.svg";
    }
}

function toggleConfirmPasswordVisibility() {
    var passwordInput = document.getElementById("confirmPassword");
    var eyeIcon = document.getElementById("confirmEyeIcon");
    if (passwordInput.type === "password") {
        passwordInput.type = "text";
        eyeIcon.src = "/theme/images/eyeopen.svg";
    } else {
        passwordInput.type = "password";
        eyeIcon.src = "/theme/images/eyeclose.svg";
    }
}


