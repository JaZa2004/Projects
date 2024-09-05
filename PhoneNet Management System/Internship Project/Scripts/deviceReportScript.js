var app = angular.module('DeviceReportApp', []);
app.controller('DeviceReportController', function ($http, $scope) {
    $scope.statistics = [];
    $scope.filteredstatistics = [];
    $scope.status = "";
    $scope.deviceFilter = null;
    $scope.loading = true;

    setTimeout(function () {
        $http.get("/api/Report/getDeviceStatistics")
            .then(function (response) {
                $scope.statistics = response.data;
                $scope.filteredstatistics = $scope.statistics;
                $scope.loading = false;
            });
    }, 1000);
    $scope.filter = function () {
        $scope.loading = true;
        DeviceFilter = $scope.deviceFilter == null ? -1 : $scope.deviceFilter.id;
        PhoneStatus = $scope.status == "" ? -1 : $scope.status;
        setTimeout(function () {
            $http.get("/api/Report/getFilteredDeviceStatistics", { params: { status: PhoneStatus, DeviceId: DeviceFilter } })
                .then(function (response) {
                    $scope.filteredstatistics = response.data;
                    $scope.loading = false;
                });
        }, 1000);
    };
});