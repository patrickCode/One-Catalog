angular.module("common", []);

angular.module("shell", ["common"]);
angular.module("catalog", ["common"]);

angular.module("msonecatalog", ["ui.router", "AdalAngular", "shell", "catalog", "common"]);

angular.module("msonecatalog")
    .config(['$stateProvider', '$urlRouterProvider', 'adalAuthenticationServiceProvider', '$httpProvider',
        function ($stateProvider, $urlRouterProvider, adalProvider, $httpProvider) {

            $urlRouterProvider.otherwise('/home/catalog');

            $stateProvider
                .state('home', {
                    url: '/home',
                    templateUrl: '/templates/shell/shell.html',
                    controller: "shellController",
                    requireADLogin: true
                })
                    .state('home.catalog', {
                        url: '/catalog',
                        templateUrl: '/templates/catalog/catalog.html',
                        controller: "catalogController",
                        requireADLogin: true
                    })
                        .state('home.details', {
                            url: '/{id}/details',
                            templateUrl: '/templates/catalog/detail.html',
                            controller: "catalogDetailsController",
                            requireADLogin: true
                        });

            adalProvider.init({
                instance: 'https://login.microsoftonline.com/',
                tenant: 'microsoft.onmicrosoft.com',
                clientId: 'aa870ebb-a3ab-49ea-a716-47e1e24de224',
                //extraQueryParameter: 'nux=1',
                cacheLocation: 'localStorage', // enable this for IE, as sessionStorage does not work for localhost.
                //endpoints: selfEndpoint
            }, $httpProvider);
        }]);

