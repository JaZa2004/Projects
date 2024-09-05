app.directive('phoneNumberStatusSelector', function ($http) {
    return {
        restrict: 'E',
        scope: {
            selectedStatus: '=',
            onChange: '&',
        },
        template: `
        <select  ng-model="selectedStatus">
                            <option value="">All PhoneNumbers</option>
                            <option value="1">Reserved</option>
                            <option value="2">NonReserved</option>
                        </select>
        `,
        controller: function ($scope) {
            $scope.$watch('selectedStatus', function (newValue, oldValue) {
                if (newValue !== oldValue) {
                    $scope.onChange();
                }
            })
        },
    }
});