var app = angular.module('PhoneNumbersApp', ['ui.bootstrap']);
app.controller('PhoneNumbersController', function ($scope, $http, $uibModal) {
    $scope.phonenumbers = [];
    $scope.filteredphonenumbers = [];
    $scope.searchNumber = '';
    $scope.searchDevice = null;
    $scope.fadeOut = false;
    $scope.notificationMessage = '';
    $scope.loading = true;

    $scope.showNotification = function (message) {
        $scope.notificationMessage = message;
        $scope.fadeOut = false;
        setTimeout(function () {
            $scope.$apply(function () {
                $scope.fadeOut = true; // Trigger fade out
            });
        }, 3000);
    };

    setTimeout(function () {
        $http.get("/api/PhoneNumbers/getAllPhoneNumbers")
            .then(function (response) {
                $scope.phonenumbers = response.data;
                $scope.filteredphonenumbers = $scope.phonenumbers;
                $scope.loading = false;
            });
    },1000);

    $scope.filter = function () {
        $scope.loading = true;
        var deviceId = $scope.searchDevice == null ? -1 : $scope.searchDevice.id;
        setTimeout(function () {
            $http.get("/api/PhoneNumbers/getFilteredPhoneNumbers", {
                params: {
                    Number: $scope.searchNumber,
                    DeviceId: deviceId
                }
            }).then(function (response) {
                $scope.filteredphonenumbers = response.data;
                $scope.loading = false;
            }).catch(function (error) {
                console.error('Error occurred:', error);
            });
        }, 1000);
    };


    $scope.openModal = function (phonenumber) {
        var modalInstance = $uibModal.open({
            templateUrl: 'PhoneNumberModal.html',
            controller: 'PhoneModalCtrl',
            resolve: {
                phonenumber: function () {
                    return phonenumber ? angular.copy(phonenumber) : null;
                }
            }
        });

        modalInstance.result.then(function (pn) {
            if (!phonenumber) {
                $http.post("/api/PhoneNumbers/addPhoneNumber", pn)
                    .then(function (response) {
                        $scope.phonenumbers = response.data;
                        $scope.filteredphonenumbers = $scope.phonenumbers;
                        $scope.showNotification("PhoneNumber Added Successfully");

                    }, function (error) {
                        console.error('Error adding PhoneNumber:', error);
                    });
            } else {
                $http.put("/api/PhoneNumbers/updatePhoneNumber", pn)
                    .then(function (response) {
                        $scope.phonenumbers = response.data;
                        $scope.filteredphonenumbers = $scope.phonenumbers;
                        $scope.showNotification("PhoneNumber Updated Successfully");
                    })
                    .catch(function (error) {
                        console.error("Error updating Phone Number:", error);
                    });
            }
        }, function () {
            console.log('Modal dismissed');
        });
    };
});

app.controller('PhoneModalCtrl', function ($scope, $http, $uibModalInstance, phonenumber) {
    $scope.Title = phonenumber ? 'Edit Phone Number' : 'Add a New Phone Number';
    $scope.phoneNumber = angular.copy(phonenumber) || { Number: '', Device: '' };
    $scope.errorMessageDevice = "";
    $scope.errorMessageNumber = "";
    $scope.clicked = false;
    $scope.DuplicateFound = false;
    $scope.ButtonDisable = false;

    $scope.DisableButton = function () {
        $scope.ButtonDisable = false;
    }

    $scope.save = function () {
        $scope.clicked = true;
        $scope.errorMessageDevice = "";
        $scope.errorMessageNumber = "";
        if (!$scope.phoneNumber.Device || !$scope.phoneNumber.Number) {
            if (!$scope.phoneNumber.Number) {
                $scope.errorMessageNumber = "* Phone Number is required";
            };
            if (!$scope.phoneNumber.Device) {
                $scope.errorMessageDevice = "* Device Name is required";
            };
            return;
        };
        if (!phonenumber) {
            $http.get("/api/PhoneNumbers/CheckDuplicatePhoneNumber", { params: { Number: $scope.phoneNumber.Number } })
                .then(function (response) {
                    if (response.status === 200) {
                        $uibModalInstance.close($scope.phoneNumber);
                    };
                }, function (error) {
                    if (error.status === 409) {
                        $scope.DuplicateFound = true;
                        $scope.ButtonDisable = true;
                    } else {
                        $scope.DuplicateFound = true;
                        $scope.ButtonDisable = true;
                    }
                });
        }
        else {
            if ($scope.phoneNumber.Number != phonenumber.Number) {
                $http.get("/api/PhoneNumbers/CheckDuplicatePhoneNumber", { params: { Number: $scope.phoneNumber.Number } })
                    .then(function (response) {
                        if (response.status === 200) {
                            $uibModalInstance.close($scope.phoneNumber);
                        };
                    }, function (error) {
                        if (error.status === 409) {
                            $scope.DuplicateFound = true;
                            $scope.ButtonDisable = true;

                        } else {
                            $scope.DuplicateFound = true;
                            $scope.ButtonDisable = true;

                        }
                    });
            }
            else {
                $uibModalInstance.close($scope.phoneNumber);
            }
        }
    };


    $scope.cancel = function () {
        $uibModalInstance.dismiss('cancel');
    };
});
