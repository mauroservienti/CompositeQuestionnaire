(function () {
    angular.module('app.controllers')
        .controller('questionsController',
        ['$log', 'endpoints.config', '$http',
            function ($log, config, $http) {

                var ctrl = this;

                ctrl.isLoading = null;
                ctrl.questions = null;

                ctrl.refreshQuestions = function () {
                    ctrl.isLoading = $http.get(config.gatewayBaseUrl + '/questions/')
                        .then(function (response) {
                            ctrl.questions = response.data.questions;
                            $log.debug('questions', ctrl.questions);
                        })
                        .catch(function (error) {
                            $log.error('Something went wrong: ', error);
                            ctrl.loadError = 'Something went wrong.';
                        });
                };

                ctrl.refreshQuestions();

            }]);
}())