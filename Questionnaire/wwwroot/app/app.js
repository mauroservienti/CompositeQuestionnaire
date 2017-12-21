/* global angular */
(function () {

    'use strict';

    angular.module('app.controllers', []);
    angular.module('app.services', []);
    angular.module('app.directives', []);

    var app = angular.module('app', [
        'ngRoute',
        'ui.router',
        'cgBusy',
        'app.controllers',
        'app.services',
        'app.directives'
    ]);

    angular.module('app.services')
        .constant('endpoints.config', {
            gatewayBaseUrl: 'http://localhost:58682',
            localApiUrl: 'http://localhost:58676/api'
        });

    app.run(['$log', '$rootScope',
        function ($log, $rootScope) {
            $rootScope.$log = $log;
            console.debug('app run.');
        }]);

}())