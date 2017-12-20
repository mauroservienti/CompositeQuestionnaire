(function () {

    angular.module('app.services')
        .config(['$stateProvider', '$locationProvider', '$logProvider',
        function ($stateProvider, $locationProvider, $logProvider) {

                $logProvider.debugEnabled(true);
                $locationProvider.html5Mode(false);

                var rootViews = {
                    '': {
                        templateUrl: '/app/presentation/questionsView.html',
                        controller: 'questionsController',
                        controllerAs: 'ctrl'
                    }
                };

                $stateProvider
                    .state('root', {
                        url: '',
                        views: rootViews
                    })
                    .state('questions', {
                        url: '/',
                        views: rootViews
                    })
                    .state('addNewQuestion', {
                        url: 'questions/new',
                        views: {
                            '': {
                                templateUrl: '/app/presentation/newQuestionView.html',
                                controller: 'newQuestionController',
                                controllerAs: 'ctrl'
                            }
                        }
                    });

            }]);
}())