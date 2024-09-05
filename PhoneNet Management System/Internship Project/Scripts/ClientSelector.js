app.directive('clientSelector', function ($http) {
    return {
        restrict: 'E',
        scope: {
            selectedClient: '=',
            onFilterChange: '&'        },
        template: `
            <select ng-model="selectedClient" ng-options="client as client.Name for client in clients track by client.Id">
                <option value="">All Clients</option>
            </select>
        `,
        controller: function ($scope) {
            $scope.clients = [];

            $http.get("/api/Clients/getAllClients")
                .then(function (response) {
                    $scope.clients = response.data;
                })
                .catch(function (error) {
                    console.error('Error fetching clients:', error);
                });

            $scope.$watch('selectedClient', function (newValue, oldValue) {
                if (newValue !== oldValue) {
                    $scope.onFilterChange();
                }
            });

            $scope.getData = function () {
                return $scope.selectedClient;
            };
        }
    };
});
