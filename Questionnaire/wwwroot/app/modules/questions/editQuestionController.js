(function () {
    angular.module('app.controllers')
        .controller('editQuestionController',
        ['$log', 'endpoints.config', '$http', '$state', 'question', 'uniqueIndentifiersService',
            function ($log, config, $http, $state, question, uniqueIndentifiers) {

                $log.debug('question', question);

                var ctrl = this;

                ctrl.isLoading = null;
                ctrl.questionId = question.questionId;
                ctrl.questionText = question.questionText;
                ctrl.answers = [];

                for (var i = 0; i < question.answers.length; i++) {
                    var a = question.answers[i];
                    ctrl.answers.push({
                        questionId: a.questionId,
                        answerId: a.answerId,
                        answerText: a.answerText,
                    });
                }

                ctrl.correctAnswerId = question.rule.correctAnswerId;

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

                //ctrl.saveQuestion = function () {

                //    $log.debug('questionId', ctrl.questionId);
                //    $log.debug('questionText', ctrl.questionText);
                //    $log.debug('answers', ctrl.answers);
                //    $log.debug('correctAnswerId', ctrl.correctAnswerId);

                //    var newQuestion = {
                //        questionId: ctrl.questionId,
                //        questionText: ctrl.questionText,
                //        answers: ctrl.answers,
                //        correctAnswerId: ctrl.correctAnswerId
                //    };

                //    ctrl.isLoading = $http.put(config.gatewayBaseUrl + '/questions', newQuestion)
                //        .then(function (response) {
                //            $state.go('questions');
                //        })
                //};
            }]);
}())