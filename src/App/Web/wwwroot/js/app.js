angular.module("common", []);

angular.module("user", ["common"]);
angular.module("technology", ["common"])
angular.module("shell", ["common", "user", "technology"]);
angular.module("catalog", ["common"]);


angular.module("msonecatalog", ["ui.router", "AdalAngular", "ngTagsInput", "shell", "catalog", "technology", "common"]);

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
                        })
            .state('home.myCatalogue', {
                url: "/mycatalog",
                templateUrl: "/templates/catalog/userCatalog.html",
                controller: "userCatalogController"
            })
            .state('home.addProject', {
                url: "/add",
                templateUrl: "/templates/catalog/addProject.html",
                controller: "addProjectController"
            })
            .state('home.editProject', {
                url: "/{id}/edit",
                templateUrl: "/templates/catalog/addProject.html",
                controller: "editProjectController"
            });

            var enpoints = {
                "api": "aa870ebb-a3ab-49ea-a716-47e1e24de224",
                'https://graph.windows.net': 'aa870ebb-a3ab-49ea-a716-47e1e24de224'
            };

            adalProvider.init({
                instance: 'https://login.microsoftonline.com/',
                tenant: 'microsoft.onmicrosoft.com',
                clientId: 'aa870ebb-a3ab-49ea-a716-47e1e24de224',
                //extraQueryParameter: 'nux=1',
                cacheLocation: 'localStorage', // enable this for IE, as sessionStorage does not work for localhost.
                //endpoints: enpoints
            }, $httpProvider);
        }]);

