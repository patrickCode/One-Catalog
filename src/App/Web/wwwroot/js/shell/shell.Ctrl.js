(function (module) {

    var shellController = function ($scope, $rootScope, $state, $stateParams, adalAuthenticationService, userService) {

        var goTo = function (page) {

        }

        var search = function () {
            alert($scope.catalog.searchtext);
        }

        var filterApplied = function (selectedFacet) {
            $rootScope.$emit("filter.applied", { selectedFacet: selectedFacet, selectedFilters: $scope.facetmodel });
        }

        var filtersChanged = function (evt, facetResponse) {
            //$scope.facets = facetResponse.facets;
            if (facetResponse.selectedFacet !== undefined) {
                //TODO - When more filters appear
                //_.each($scope.facets, function (facet) {
                //    if (facetResponse.selectedFacet !== facet.name)
                //        facet.filters = facetResponse.facets[facet.name].filters;
                //});
            }
            else {
                $scope.facets = facetResponse.facets;
                //if ($scope.originalResponse === null) {
                //    $scope.facets = facetResponse.facets;
                //    $scope.originalResponse = facetResponse.facets;
                //} else {
                //    $scope.facets = $scope.originalResponse;
                //}
            }
        }

        var getUsers = function () {
            userService.getUserDetail();
        }

        var init = function () {
            $rootScope.breadcrumbs = [{
                title: "",
                href: ""
            }];
            $scope.userInfo = adalAuthenticationService.userInfo;

            $scope.goTo = goTo;
            $scope.search = search;
            $scope.filterApplied = filterApplied;
            $scope.catalog = {
                searchtext: ""
            }
            $rootScope.$on('filter.changed', filtersChanged);

            $scope.state = $state;

            
            $scope.facets = [];
            $scope.facetmodel = [[]];
            //Hack since only 1 Facet is present
            $scope.originalResponse = null;
            $scope.getUsers = getUsers;
        }
        init();
    }

    module.controller("shellController", ['$scope', '$rootScope', '$state', '$stateParams', 'adalAuthenticationService', 'userService', shellController])

}(angular.module("shell")))