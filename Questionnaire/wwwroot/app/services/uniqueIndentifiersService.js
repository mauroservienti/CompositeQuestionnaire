(function () {
    angular.module('app.services')
        .factory('uniqueIndentifiersService',
        ['$log', 'endpoints.config', '$http',
            function ($log, config, $http) {

                var getNewGuid = function () {
                    return $http.get(config.localApiUrl + '/UniqueIndentifiers/NewGuid')
                        .then(function (response) {
                            return response.data.uniqueIndentifier;
                        })
                };

                var svc = {
                    newGuid: getNewGuid
                };

                return svc;
            }]);
}())