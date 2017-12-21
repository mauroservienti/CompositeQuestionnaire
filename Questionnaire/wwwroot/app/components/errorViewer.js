(function () {
    angular.module('app.directives')
        .directive('errorViewer', function () {
            return {
                restrict: 'E',
                scope: {
                    error: '@'
                },
                templateUrl: '/app/components/errorViewer.html'
            };
        });
}())