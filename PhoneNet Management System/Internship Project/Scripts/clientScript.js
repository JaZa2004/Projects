var app = angular.module('clientApp', ['ui.bootstrap']);

app.controller('ClientController', function ($scope, $http, $uibModal, $timeout) {
    $scope.clients = [];
    $scope.filteredClients = [];
    $scope.typeSearchValue = "";
    $scope.searchtext = "";
    $scope.fadeOut = false;
    $scope.notificationMessage = '';
    $scope.loading = false;
    $scope.disabledIndex = null;

    $scope.isDisabled = function (index) {
        return $scope.disabledIndex === index;
    };

    $scope.showNotification = function (message) {
        $scope.notificationMessage = message;
        $scope.fadeOut = false;
        setTimeout(function () {
            $scope.$apply(function () {
                $scope.fadeOut = true;
            });
        }, 3000);
    };

    $scope.togglePhoneNumbers = function (device, index) {
        device.showPhoneNumbers = !device.showPhoneNumbers;
        if (device.showPhoneNumbers) {
            $scope.disabledIndex = index;
        } else {
            $scope.disabledIndex = null;
        }
    };


    function fetchAllClients() {
        $scope.loading = true;
        setTimeout(function () {
            $http.get('/api/clients/getAllClients')
                .then(function (response) {
                    $scope.clients = response.data;
                    $scope.clients.forEach(client => client.showPhoneNumbers = false);
                    $scope.filteredClients = $scope.clients;
                    $scope.loading = false;
                });
        }, 1000);
    };

    fetchAllClients();

    $scope.searchClient = function () {
        $scope.loading = true;
        $http.get('/api/clients/getFilteredClients', { params: { searchtext: $scope.searchtext } })
            .then(function (response) {
                $scope.filteredClients = response.data;
                $scope.loading = false;
            });
    };

    $scope.TypeSearch = function () {
        $scope.loading = true;
        $timeout(function () {
            $http.get('/api/clients/getFilteredTypeClients', { params: { typeInt: $scope.typeSearchValue ? $scope.typeSearchValue : -1 } })
                .then(function (response) {
                    $scope.filteredClients = response.data;
                    $scope.loading = false;
                });
        }, 1000);
    };

    $scope.openModal = function (client) {
        var modalInstance = $uibModal.open({
            templateUrl: 'clientModal.html',
            controller: 'ClientModalCtrl',
            resolve: {
                client: function () {
                    return client ? angular.copy(client) : {};
                }
            }
        });

        modalInstance.result.then(function (resultClient) {
            if (!client) {
                $http.post('/api/clients/addClient', resultClient)
                    .then(function (response) {
                        $scope.clients = response.data;
                        $scope.filteredClients = $scope.clients;
                        $scope.showNotification("Client Added Successfully");

                    });
            } else {
                $http.put('/api/clients/updateClient', resultClient)
                    .then(function (response) {
                        $scope.clients = response.data;
                        $scope.filteredClients = $scope.clients;
                        $scope.showNotification("Client Updated Successfully");
                    });
            }
        });
    };

    $scope.openReserveModal = function (client) {
        var modalInstance = $uibModal.open({
            templateUrl: 'ReserveModal.html',
            controller: 'ReserveModalCtrl',
            resolve: {
                client: function () {
                    return client ? angular.copy(client) : {};
                }
            }
        });

        modalInstance.result.then(function (phoneNbId) {
            var reservationData = {
                ClientId: client.Id,
                PhoneNumberId: phoneNbId
            };
            $http.post('/api/Reservations/addReservation', reservationData)
                .then(function () {
                    $http.get('/api/clients/getAllClients')
                        .then(function (response) {
                            $scope.clients = response.data;
                            $scope.filteredClients = $scope.clients;
                            $scope.showNotification("PhoneNumber Reserved Successfully");
                        });
                });
        });
    };

    $scope.openUnreserveModal = function (client) {
        var modalInstance = $uibModal.open({
            templateUrl: 'UnreserveModal.html',
            controller: 'UnreserveModalCtrl',
            resolve: {
                client: function () {
                    return client ? angular.copy(client) : {};
                }
            }
        });

        modalInstance.result.then(function (phoneNbId) {
            var UnreservationData = {
                ClientId: client.Id,
                PhoneNumberId: phoneNbId
            };
            $http.put('/api/Reservations/UpdateReservation', UnreservationData)
                .then(function () {
                    $http.get('/api/clients/getAllClients')
                        .then(function (response) {
                            $scope.clients = response.data;
                            $scope.filteredClients = $scope.clients;
                            $scope.showNotification("PhoneNumber UnReserved Successfully");

                        });
                });
        });
    };

});


app.controller('ClientModalCtrl', function ($scope, $uibModalInstance, client, $http) {
    $scope.client = angular.copy(client);
    $scope.modalTitle = client.Id ? 'Edit Client' : 'Add New Client';
    $scope.edit = client.Id ? true : false;
    $scope.errorMessageType = "";
    $scope.errorMessageName = "";
    $scope.errorMessageDate = "";
    $scope.clicked = false;
    $scope.DuplicateFound = false;
    $scope.ButtonDisable = false;

    $scope.DisableButton = function () {
        $scope.ButtonDisable = false;
    };

    $scope.client.Type = client.Id ? client.Type.toString() : '';
    $scope.client.BirthDate = client.BirthDate ? new Date(client.BirthDate) : null;

    $scope.save = function () {
        $scope.clicked = true;
        $scope.errorMessageType = "";
        $scope.errorMessageName = "";
        if ($scope.client.Type == '' || $scope.client.Name == null) {
            if ($scope.client.Type == '') {
                $scope.errorMessageType = "* Client Type is required ";
            };
            if ($scope.client.Name == null) {
                $scope.errorMessageName = "* Client Name is required ";
            };
            return;
        }
        if ($scope.client.BirthDate) {

            const DateObject = new Date($scope.client.BirthDate);
            $scope.client.BirthDate = new Date(DateObject.getTime() - DateObject.getTimezoneOffset() * 60000).toISOString();
            const adjustedBirthDate = new Date($scope.client.BirthDate);
            const currentDate = new Date();
            const differenceInTime = currentDate - adjustedBirthDate;
            const millisecondsInYear = 1000 * 60 * 60 * 24 * 365.25;
            const differenceInYears = differenceInTime / millisecondsInYear;
            const roundedDifferenceInYears = Math.floor(differenceInYears);
            if (roundedDifferenceInYears < 18) {
                $scope.errorMessageDate = "* Minimum age required is 18 years";
                return;
            }
        }
        if (!client.Name) {
            $http.get("/api/Clients/CheckDuplicateClient", { params: { ClientName: $scope.client.Name } })
                .then(function (response) {
                    if (response.status === 200) {
                        $uibModalInstance.close($scope.client);
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
        } else {
            if ($scope.client.Name != client.Name)
                $http.get("/api/Clients/CheckDuplicateClient", { params: { ClientName: $scope.client.Name } })
                    .then(function (response) {
                        if (response.status === 200) {
                            $uibModalInstance.close($scope.client);
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
            else {
                $uibModalInstance.close($scope.client);
            }

        }
    };

    $scope.cancel = function () {
        $uibModalInstance.dismiss('cancel');
    };
});

app.controller('ReserveModalCtrl', function ($scope, $http, $uibModalInstance, client) {
    $scope.clientName = client.Name;
    $scope.PhoneNumberReserved = '';
    $scope.phoneNumbersNotReserved = [];
    $scope.errorMessageNumber = "";
    $scope.clicked = false;

    $http.get("/api/PhoneNumbers/getNonReservedPhoneNumbers")
        .then(function (response) {
            $scope.phoneNumbersNotReserved = response.data;
        });

    $scope.save = function () {
        $scope.clicked = true;
        $scope.errorMessageNumber = "";
        if ($scope.PhoneNumberReserved == '') {
            $scope.errorMessageNumber = "* Phone Number is Required";
            return;
        }
        $uibModalInstance.close($scope.PhoneNumberReserved.Id);
    };

    $scope.cancel = function () {
        $uibModalInstance.dismiss('cancel');
    };
});


app.controller('UnreserveModalCtrl', function ($scope, $http, $uibModalInstance, client) {
    $scope.clientName = client.Name;
    $scope.PhoneNumberUnreserved = '';
    $scope.phoneNumbersReservedByClient = [];
    $scope.errorMessageNumber = "";
    $scope.clicked = false;
    $http.get("/api/PhoneNumbers/getPhoneNumbersReservedByClient", { params: { Id: client.Id } })
        .then(function (response) {
            $scope.phoneNumbersReservedByClient = response.data;
        });
    $scope.save = function () {
        $scope.clicked = true;
        $scope.errorMessageNumber = "";
        if ($scope.PhoneNumberUnreserved == '') {
            $scope.errorMessageNumber = "* Phone Number is Required";
            return;
        }
        $uibModalInstance.close($scope.PhoneNumberUnreserved.Id);
    };

    $scope.cancel = function () {
        $uibModalInstance.dismiss('cancel');
    };
});
