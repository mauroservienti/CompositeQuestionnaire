(function () {
    angular.module('app.controllers')
        .controller('newQuestionController',
        ['$log', 'endpoints.config', '$http', '$state',
            function ($log, config, $http, $state) {

                var ctrl = this;

                ctrl.isLoading = null;
                ctrl.questionId = '';
                ctrl.questionText = '';
                ctrl.answers = [];
                ctrl.correctAnswerId = '';

                var getNewGuid = function () {
                    return $http.get(config.localApiUrl + '/UniqueIndentifiers/NewGuid')
                        .then(function (response) {
                            return response.data.uniqueIndentifier;
                        })
                };

                ctrl.addAnswer = function () {

                    ctrl.isLoading = getNewGuid()
                        .then(function (guid) {

                            ctrl.answers.push({
                                questionId: ctrl.questionId,
                                answerId:  guid,
                                answerText: ''
                            });

                            $log.debug(ctrl.answers);
                        });
                };

                ctrl.addNewQuestion = function () {

                    $log.debug('questionId', ctrl.questionId);
                    $log.debug('questionText', ctrl.questionText);
                    $log.debug('answers', ctrl.answers);
                    $log.debug('correctAnswerId', ctrl.correctAnswerId);

                    var newQuestion = {
                        questionId: ctrl.questionId,
                        questionText: ctrl.questionText,
                        answers: ctrl.answers,
                        correctAnswerId: ctrl.correctAnswerId
                    };

                    ctrl.isLoading = $http.put(config.gatewayBaseUrl + '/questions', newQuestion)
                        .then(function (response) {
                            $state.go('questions');
                        })
                };

                ctrl.isLoading = getNewGuid()
                    .then(function (guid) {
                        ctrl.questionId = guid;

                        $log.debug(ctrl.questionId);
                    });
            }]);
}())