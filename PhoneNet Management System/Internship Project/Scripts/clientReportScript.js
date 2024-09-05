var app = angular.module('ClientReportApp', []);
app.controller('ClientReportController', function ($http, $scope) {
    $scope.statistics = [];
    $scope.filteredstatistics = [];
    $scope.ClientType = "";
    $scope.loading = true;

    setTimeout(function () {
        $http.get("/api/Report/getClientTypeStatistics")
            .then(function (response) {
                $scope.statistics = response.data;
                $scope.filteredstatistics = $scope.statistics;
                $scope.loading = false;
            });
    }, 1000);
    $scope.filter = function () {
        $scope.loading = true;
        clienttype = $scope.ClientType == "" ? -1 : $scope.ClientType;
        setTimeout(function () {
            $http.get("/api/Report/getFilteredClientTypeStatistics", { params: { ClientType: clienttype } })
                .then(function (response) {
                    $scope.filteredstatistics = response.data;
                    $scope.loading = false;
                });
        }, 1000);
    };
});