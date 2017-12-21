(function () {
    angular.module('app.controllers')
        .controller('newQuestionController',
        ['$log', 'endpoints.config', '$http', '$state', 'uniqueIndentifiersService',
            function ($log, config, $http, $state, uniqueIndentifiers) {

                var ctrl = this;

                ctrl.isLoading = null;
                ctrl.questionId = '';
                ctrl.questionText = '';
                ctrl.answers = [];
                ctrl.correctAnswerId = '';

                ctrl.addAnswer = function () {

                    ctrl.isLoading = uniqueIndentifiers.newGuid()
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

                ctrl.isLoading = uniqueIndentifiers.newGuid()
                    .then(function (guid) {
                        ctrl.questionId = guid;

                        $log.debug(ctrl.questionId);
                    });
            }]);
}())