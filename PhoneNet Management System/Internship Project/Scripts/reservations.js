var app = angular.module('ReservationsApp',[]);
app.controller('ReservationsController', function ($scope, $http) {
    $scope.reservations = [];
    $scope.filteredreservations = [];
    $scope.searchClient = null;
    $scope.searchphoneNumber = null;
    $scope.phoneNumbers = [];
    $scope.clients = [];
    $scope.loading = true;

    setTimeout(function () {
        $http.get("/api/Reservations/getAllReservations")
            .then(function (response) {
                $scope.reservations = response.data;
                $scope.filteredreservations = $scope.reservations;
                $scope.loading = false;
            });
    },1000);

    $http.get("/api/Clients/getAllClients")
        .then(function (response) {
            $scope.clients = response.data;
        });

    $http.get("/api/PhoneNumbers/getAllPhoneNumbers")
        .then(function (response) {
            $scope.phoneNumbers = response.data;
        });

    $scope.filter = function () {
        $scope.loading = true;
        var clientId = $scope.searchClient == null ? -1 : $scope.searchClient.Id;
        var phoneNumberSelector = angular.element(document.querySelector('phone-number-selector')).isolateScope();
        var phoneId = phoneNumberSelector.getData() ? phoneNumberSelector.getData().Id : -1;

        setTimeout(function () {
            $http.get("/api/Reservations/getFilteredReservations", {
                params: {
                    clientId: clientId,
                    PhoneNumberId: phoneId
                }
            }).then(function (response) {
                $scope.filteredreservations = response.data;
                $scope.loading = false;
            });
        },1000);
    };

    $scope.AddReservation = function (ClId, PhId, BEDate) {
        $http.post("/api/Reservations/AddReservation", { params: { clientId: ClId, PhoneNumberId: PhId, BED: BEDate } });
    };

    $scope.UpdateReservation = function (ClId, PhId, EEDate) {
        $http.put("/api/Reservations/UpdateReservation", { params: { clientId: ClId, PhoneNumberId: PhId, EED: EEDate } })
    };
});
