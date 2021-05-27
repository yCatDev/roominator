window.onload = funonload;

function funonload() {
    var summary = document.getElementById("RegisterSummary").childNodes[0].childNodes[0].innerHTML;
    if (summary.length != "") {
        if (summary=="Passwords must have at least one digit ('0'-'9').") {
            summary = "Пароль повинен мати хоча б одну цифру";
        }
        if (summary =="Passwords must have at least one lowercase ('a'-'z').") {
            summary = "Пароль повинен мати хоча б одну маленьку літеру";
        }

        if (summary =="Passwords must have at least one uppercase ('A'-'Z').") {
            summary = "Пароль повинен мати хоча б одну велику літеру";
        }
        if (summary == "Passwords must have at least one non alphanumeric character.") {
            summary = "Пароль повинен мати хоча б один спецсимвол";
        }
        SetErrorLine(summary, true)
    }

    var target = document.getElementById("RegAcc");

    const config = {
        childList: true,
        subtree: true
    };

    const callback = function (mutationsList, observer) {
        var errors = [];
        //Ошибка в мыле
        if (document.getElementById("SignupMail-error")) {
            SetWarningIcon("SignupMailWarning", true)
            errors.push(document.getElementById("SignupMail-error").innerHTML);
        }
        else {
            SetWarningIcon("SignupMailWarning", false)
        }
        //Ошибка в пароле
        if (document.getElementById("SignupPassword-error")) {
            SetWarningIcon("SignupPassworfWarning", true)
            errors.push(document.getElementById("SignupPassword-error").innerHTML);
        }
        else {
            SetWarningIcon("SignupPassworfWarning", false)
        }
        //Ошибка в пароле 2
        if (document.getElementById("SignupConfirmPassword-error")) {
            SetWarningIcon("SignupConfirmPasswordWarning", true)
            errors.push(document.getElementById("SignupConfirmPassword-error").innerHTML);
        }
        else {
            SetWarningIcon("SignupConfirmPasswordWarning", false)
        }

        var mql = window.matchMedia("(orientation: landscape)")
        if (errors.length > 0) {
            SetErrorLine(errors.join( (mql) ? (" | ") : ("<br>")), true)
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
    elem = document.getElementById("RegisterErrLine");

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