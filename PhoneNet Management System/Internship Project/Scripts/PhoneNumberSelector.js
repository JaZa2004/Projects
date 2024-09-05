app.directive('phoneNumberSelector', function ($http) {
    return {
        restrict: 'E',
        scope: {
            selectedPhoneNumber: '=',
            onFilterChange: '&?',
            phoneNumbers : '='
        },
        template: `
            <select ng-model="selectedPhoneNumber" ng-options="phoneNumber as phoneNumber.Number for phoneNumber in phoneNumbers track by phoneNumber.Id">
                <option value="" ng-disabled="DisableOption">{{placeholder}}</option>
            </select>
        `,
        controller: function ($scope) {
            $scope.placeholder = $scope.onFilterChange ? "All Phone Numbers" : "Select Phone Number";
            $scope.DisableOption = $scope.onFilterChange ? false : true;
            $scope.$watch('selectedPhoneNumber', function (newValue, oldValue) {
                if (newValue !== oldValue) {
                    $scope.onFilterChange();
                }
            });

            $scope.getData = function () {
                return $scope.selectedPhoneNumber;
            };
        }
    };
});
