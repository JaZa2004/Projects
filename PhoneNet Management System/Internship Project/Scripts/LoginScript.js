var app = angular.module('LoginApp', []);
app.controller('LoginController', function ($http, $scope, $window) {
    $scope.Username = "";
    $scope.Password = "";
    $scope.errorMessageGeneral = "";
    $scope.errorMessageUsername = "";
    $scope.errorMessagePassword = "";
    $scope.clicked = false;
    $scope.login = function () {
        $scope.errorMessageUsername = "";
        $scope.errorMessagePassword = "";
        $scope.errorMessageGeneral = "";
        $scope.clicked = true;
        if ($scope.Username === "" || $scope.Password === "") {
            if ($scope.Username === "") {
                $scope.errorMessageUsername = "* Username is Required";
            }
            if ($scope.Password === "") {
                $scope.errorMessagePassword = "* Password is Required";
            }
            return;
        }

        $http.post('/api/login/authenticate', { username: $scope.Username, password: $scope.Password })
            .then(function (response) {
                if (response.data.success) {
                    $scope.errorMessageGeneral = "";
                    $scope.Username = "";
                    $scope.Password = "";
                    $window.location.href = '/Home/Devices';
                } else {
                    $scope.errorMessageGeneral = "* Invalid login attempt.";
                    $scope.Username = "";
                    $scope.Password = "";
                }
            }, function () {
                $scope.errorMessageGeneral = "* Error occurred during login.";
            });
    };
});

app.controller('RegisterController', function ($http, $scope, $window) {
    $scope.Username = "";
    $scope.Password = "";
    $scope.ConfirmPassword = "";
    $scope.errorMessageGeneral = "";
    $scope.errorMessageUsername = "";
    $scope.errorMessagePassword = "";
    $scope.errorMessageConfirmPassword = "";
    $scope.clicked = false;

    $scope.register = function () {
        $scope.errorMessageGeneral = "";
        $scope.errorMessageUsername = "";
        $scope.errorMessagePassword = "";
        $scope.errorMessageConfirmPassword = "";
        $scope.clicked = true;
        if ($scope.Username === "" || $scope.Password === "" || $scope.ConfirmPassword === "") {
            if ($scope.Username === "") {
                $scope.errorMessageUsername = "* Username is Required";
            }
            if ($scope.Password === "") {
                $scope.errorMessagePassword = "* Password is Required";
            }
            if ($scope.ConfirmPassword === "") {
                $scope.errorMessageConfirmPassword = "* Confirmed Password is Required";
            }
            return;
        }
        if ($scope.Password !== $scope.ConfirmPassword) {
            $scope.errorMessageGeneral = "* Passwords don't match.";
            return;
        }
        $http.post('/api/login/register', { username: $scope.Username, password: $scope.Password })
            .then(function (response) {
                if (response.status === 200) {
                    $scope.errorMessageGeneral = "";
                    $scope.errorMessageUsername = "";
                    $scope.errorMessagePassword = "";
                    $scope.errorMessageConfirmPassword = "";
                    $scope.Username = "";
                    $scope.Password = "";
                    $scope.ConfirmPassword = "";
                    $window.location.href = '/Home/Devices';
                } else if (response.status === 409) {
                    $scope.errorMessageGeneral = "* Username is taken by another user.";
                } else {
                    $scope.errorMessageGeneral = "* Registration failed.";
                }
            }, function (error) {
                if (error.status === 409) {
                    $scope.errorMessageGeneral = "* Username is taken by another user.";
                } else {
                    $scope.errorMessageGeneral = "* Error occurred during registration.";
                }
            });
    };
});
