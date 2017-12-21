(function () {
    angular.module('app.directives')
        .directive('questionsList', function () {
            return {
                restrict: 'E',
                scope: {
                    questions: '='
                },
                templateUrl: '/app/modules/questions/questionsList.html',
                //controller: ['$scope', '$log',
                //    function questionsListController($scope, $log) {
                //        $log.debug('$scope', $scope);
                //}],
            };
        });
}())