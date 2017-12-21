(function () {
    angular.module('app.directives')
        .directive('questionAnswersList', function () {
            return {
                restrict: 'E',
                scope: {
                    question: '='
                },
                templateUrl: '/app/modules/answers/questionAnswersList.html',
                //controller: ['$scope', '$log',
                //    function questionsListController($scope, $log) {
                //        $log.debug('$scope', $scope);
                //}],
            };
        });
}())