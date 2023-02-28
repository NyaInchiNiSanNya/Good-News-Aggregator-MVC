function readCookie(name) {
        var matches = document.cookie.match(new RegExp(
    "(?:^|; )" + name.replace(/([\.$?*|{ }\(\)\[\]\\\/\+^])/g, '\\$1') + "=([^;]*)"
        ));

    return matches ? decodeURIComponent(matches[1]) : undefined;
}
function GetCookieName() {
    let el = document.getElementById("NameField");
    el.textContent = readCookie("Name");
}
