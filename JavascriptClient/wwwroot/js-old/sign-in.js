var createNonce = function () {
    return 'ReturnSomeValueNonce';
};

var createState = function () {
    return 'ReturnSomeValueState';
};

var signIn = function () {
    var redirectUri = 'http://localhost:6004/Home/SignIn';
    var responseType = 'id_token token';
    var scope = 'openid ApiOne';
    var authUrl = '/connect/authorize/callback' +
        '?client_id=client_id_js' +
        '&redirect_uri=' + encodeURIComponent(redirectUri) +
        '&response_type=' + encodeURIComponent(responseType) +
        '&scope=' + encodeURIComponent(scope) +
        '&nonce=' + createNonce() +
        '&state=' + createState();
    var returnUrl = encodeURIComponent(authUrl);
    window.location.href = 'http://localhost:7000/Auth/Login?ReturnUrl=' + returnUrl;
};