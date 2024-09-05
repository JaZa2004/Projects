var app = angular.module('deviceApp', ['ui.bootstrap']);

app.controller('DeviceController', function ($scope, $http, $uibModal) {
    $scope.devices = [];
    $scope.filteredDevices = [];
    $scope.searchtext = '';
    $scope.currentId = 0;
    $scope.notificationMessage = '';
    $scope.fadeOut = false; // Flag for fade out
    $scope.loading = true;
    EditDisable = true;
    $scope.disabledIndex = null;

    $scope.isDisabled = function (index) {
        return $scope.disabledIndex === index;
    };

    setTimeout(function () {
        $http.get("/api/devices/getAllDevices")
            .then(function (response) {
                $scope.devices = response.data;
                $scope.devices.forEach(device => device.showPhoneNumbers = false);
                $scope.filteredDevices = $scope.devices;
                $scope.loading = false;
            });
    }, 1000);

    $scope.togglePhoneNumbers = function (device, index) {
        device.showPhoneNumbers = !device.showPhoneNumbers;
        if (device.showPhoneNumbers) {
            $scope.disabledIndex = index;
        } else {
            $scope.disabledIndex = null;
        }
    };


    $scope.DisableEditDevice = function () {
        EditDisable = true;
    };

    $scope.searchDevice = function () {
        $scope.loading = true;
        setTimeout(function () {
            $http.get("/api/devices/getFilteredDevices", { params: { searchtext: $scope.searchtext } })
                .then(function (response) {
                    $scope.filteredDevices = response.data;
                    $scope.loading = false;
                });
        }, 1000);
    };

    $scope.openModal = function (device) {
        var modalInstance = $uibModal.open({
            templateUrl: 'deviceModal.html',
            controller: 'deviceModalCtrl',
            resolve: {
                device: function () {
                    return device ? angular.copy(device) : {};
                }
            }
        });

        modalInstance.result.then(function (insertedName) {
            if (!device) {
                $http.post("/api/devices/addDevice", { Name: insertedName })
                    .then(function (response) {
                        $scope.devices = response.data;
                        $scope.filteredDevices = $scope.devices;

                        $scope.showNotification('Device added successfully!');
                    }, function (error) {
                        console.error('Error adding device:', error);
                    });
            } else {
                device.Name = insertedName;
                $http.put("/api/devices/updateDevice", device)
                    .then(function (response) {
                        $scope.devices = response.data;
                        $scope.filteredDevices = $scope.devices;
                        $scope.showNotification('Device Updated successfully!');

                    })
                    .catch(function (error) {
                        console.error("Error updating device:", error);
                    });
            }
        }, function () {
            console.log('Modal dismissed');
        });
    };

    $scope.openPhoneModal = function (phone) {
        var modalInstance = $uibModal.open({
            templateUrl: 'PhoneModal.html',
            controller: 'PhoneModalCtrl',
            resolve: {
                phone: function () {
                    return angular.copy(phone);
                }
            }
        });

        modalInstance.result.then(function (EditedNumber) {
            phone.Number = EditedNumber;
            $http.put("/api/PhoneNumbers/updatePhoneNumber", phone)
                .then(function (response) {
                    $scope.showNotification('Phone Number Updated successfully!');
                })
                .catch(function (error) {
                    console.error("Error updating PhoneNumber:", error);
                });
        }, function () {
            console.log('Modal dismissed');
        });
    };

    $scope.showNotification = function (message) {
        $scope.notificationMessage = message;
        $scope.fadeOut = false;
        setTimeout(function () {
            $scope.$apply(function () {
                $scope.fadeOut = true; // Trigger fade out
            });
        }, 3000);
    };
});

app.controller("deviceModalCtrl", function ($scope, $uibModalInstance, device, $http) {
    $scope.Title = device && device.id ? 'Edit Device' : 'Add a New Device';
    $scope.Name = device ? device.Name : '';
    $scope.errorMessageName = "";
    $scope.clicked = false;
    $scope.DuplicateFound = false;
    $scope.ButtonDisable = false;

    $scope.DisableButton = function () {
        $scope.ButtonDisable = false;
    }
    $scope.save = function () {
        $scope.clicked = true;
        $scope.errorMessageName = "";
        if ($scope.Name == null || $scope.Name.trim() === '') {
            $scope.errorMessageName = "* Device Name is required";
            return;
        }
        if (angular.equals(device, {})) {
            $http.get("/api/devices/CheckDuplicateDevice", { params: { DeviceName: $scope.Name } })
                .then(function (response) {
                    if (response.status === 200) {
                        $uibModalInstance.close($scope.Name);
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
            if ($scope.Name !== device.Name) {
                $http.get("/api/devices/CheckDuplicateDevice", { params: { DeviceName: $scope.Name } })
                    .then(function (response) {
                        if (response.status === 200) {
                            $uibModalInstance.close($scope.Name);
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
                $uibModalInstance.close($scope.Name);
            }
        }
    };

    $scope.cancel = function () {
        $uibModalInstance.dismiss('cancel');
    };
});


app.controller("PhoneModalCtrl", function ($scope, $uibModalInstance, phone, $http) {
    $scope.Title = 'Edit Phone Number';
    $scope.Number = phone.Number;
    $scope.errorMessageNumber = "";
    $scope.clicked = false;
    $scope.DuplicateFound = false;
    $scope.ButtonDisable = false;

    $scope.DisableButton = function () {
        $scope.ButtonDisable = false;
    }
    $scope.save = function () {
        $scope.clicked = true;
        $scope.errorMessageName = "";
        if ($scope.Number == null || $scope.Number.trim() === '') {
            $scope.errorMessageNumber = "* Phone Number is required";
            return;
        }

        if ($scope.Number !== phone.Number) {
            $http.get("/api/PhoneNumbers/CheckDuplicatePhoneNumber", { params: { Number: $scope.Number } })
                .then(function (response) {
                    if (response.status === 200) {
                        $uibModalInstance.close($scope.Number);
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
            $uibModalInstance.close($scope.Number);
        }
};

$scope.cancel = function () {
    $uibModalInstance.dismiss('cancel');
};
});
