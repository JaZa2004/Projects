app.directive('deviceFilter', function ($http) {
    return {
        restrict: 'E',
        scope: {
            selectedDevice: '=',
            onFilterChange: '&?'
        },
        template: `
            <select ng-model="selectedDevice" ng-options="device as device.Name for device in devices track by device.id">
                <option value="" ng-disabled="DisableOption">{{placeholder}}</option>
            </select>
        `,
        controller: function ($scope) {
            $scope.placeholder = $scope.onFilterChange ? 'All Devices' : 'Select Device';
            var selectedDeviceCopy = $scope.selectedDevice ? $scope.selectedDevice : null;
            $scope.DisableOption = $scope.onFilterChange ? false : true;
            $scope.devices = [];

            $http.get("/api/Devices/getAllDevices")
                .then(function (response) {
                    $scope.devices = response.data;
                })
                .catch(function (error) {
                    console.error('Error fetching devices:', error);
                });

            $scope.$watch('selectedDevice', function (newValue, oldValue) {
                if (newValue !== oldValue) {
                    $scope.onFilterChange();
                }
            });

            $scope.getData = function () {
                return $scope.selectedDevice;
            };
        }
    };
});
