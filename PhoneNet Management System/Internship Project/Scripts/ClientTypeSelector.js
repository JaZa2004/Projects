app.directive('clientTypeSelector', function () {
    return {
        restrict: 'E',
        scope: {
            selectedType: '=',
            onChange: '&?'
        },
        template: `
            <select ng-model="selectedType" ng-disabled="DisableSelect">
                <option value="" ng-disabled="DisableOption" ng-bind="placeholder"></option>
                <option value="0">Individual</option>
                <option value="1">Organization</option>
            </select>
        `,
        controller: function ($scope) {
            $scope.placeholder = $scope.onChange ? 'All Types' : 'Select Type';
            var selectedTypeCopy = $scope.selectedType ? $scope.selectedType : null;
            $scope.DisableOption = $scope.onChange ? false : true;
            $scope.DisableSelect = ($scope.onChange || (!$scope.onChange && selectedTypeCopy==null) )? false : true;
            $scope.$watch('selectedType', function (newValue, oldValue) {
                if (newValue !== oldValue && typeof $scope.onChange === 'function') {
                    $scope.onChange(); 
                }
            });

            $scope.getData = function () {
                return $scope.selectedType;
            };
        }
    };
});
