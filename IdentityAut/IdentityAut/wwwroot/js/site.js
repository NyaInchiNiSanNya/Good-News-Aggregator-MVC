document.addEventListener('DOMContentLoaded', function () {
   
    applyThemeFromCookieText()
});

function readCookie(name) {
    var cookies = document.cookie.split(';');
    var cookieValue = null;

    cookies.forEach(function (cookie) {
        var cookiePair = cookie.trim().split('=');
        var cookieName = cookiePair[0];

        if (cookieName === name) {
            cookieValue = cookiePair[1];
        }
    });

    return cookieValue;
}

function applyThemeFromCookie() {
    var themeValue = readCookie('theme');

    if (themeValue === 'dark') {
        document.body.style.backgroundColor = '#3b3939';
    }
}
function applyThemeFromCookieText() {
    var themeValue = readCookie('theme');

    if (themeValue === 'dark') {
        var elementsToColor = document.querySelectorAll('h5, p, label');

        for (var i = 0; i < elementsToColor.length; i++) {
            elementsToColor[i].style.color = 'white'; // Установите нужный цвет для элементов
        }
    }
}
