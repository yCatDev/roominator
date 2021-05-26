window.onload = funonload;

function funonload() {
    var summary = document.getElementById("LoginSummary").childNodes[0].childNodes[0].innerHTML;
    if (summary.length != "") {
        SetErrorLine(summary, true)
    }

    var target = document.getElementById("account");

    const config = {
        childList: true,
        subtree: true
    };

    const callback = function (mutationsList, observer) {
        var errors = [];
        //Ошибка в мыле
        if (document.getElementById("SigninMail-error")) {
            SetWarningIcon("SigninMailWarning", true)
            errors.push(document.getElementById("SigninMail-error").innerHTML);
        }
        else {
            SetWarningIcon("SigninMailWarning", false)
        }
        //Ошибка в пароле
        if (document.getElementById("SigninPassword-error")) {
            SetWarningIcon("SigninPassworfWarning", true)
            errors.push(document.getElementById("SigninPassword-error").innerHTML);
        }
        else {
            SetWarningIcon("SigninPassworfWarning", false)
        }

        var landscape = window.matchMedia("(orientation: landscape)").matches
        if (errors.length > 0) {
            SetErrorLine(errors.join((landscape) ? (" | ") : ("<br>")), true)
        }
        else {
            SetErrorLine(" ", false);
        }

    };

    const observer = new MutationObserver(callback);

    observer.observe(target, config);


}


function SetWarningIcon(id, enable) {
    elem = document.getElementById(id);
    if (enable == true) {
        if (elem.classList.contains("Hidden") || elem.classList.contains("Hide")) {
            elem.className = "AuthWarningIcon Show"
        }
    }
    else {
        if (elem.classList.contains("Show")) {
            elem.className = "AuthWarningIcon Hide"
        }
    }
}

function SetErrorLine(text, enable) {
    elem = document.getElementById("LoginErrLine");

    if (enable == true) {
        elem.innerHTML = text;
        if (elem.classList.contains("AuthCollapsedLine")
            || elem.classList.contains("AuthCollapseLine")) {
            elem.className = "AuthErrorMessage Unselectable AuthExpandLine"
        }
    }
    else {
        
        if (elem.classList.contains("AuthExpandLine")) {
            elem.className = "AuthErrorMessage Unselectable AuthCollapseLine"
        }
        elem.innerHTML = " ";
    }
}