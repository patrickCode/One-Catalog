(function (module) {

    var catalogController = function ($scope, $rootScope, $location, catalogData) {

        var search = function (reset, facet) {
            $scope.errorOcurred = false;
            $scope.loading = true;
            if (reset !== undefined && reset !== null && reset === true)
                $scope.pagination.currentPage = 1;
            catalogData.getProjects($scope.searchText, $scope.selectedFacets.technologies, $scope.pagination.currentPage, $scope.pagination.pageSize)
                .then(function (data) {
                    $scope.projects = data.results;
                    $scope.count = data.totalCount;
                    $rootScope.$emit('filter.changed', { selectedFacet: facet, facets: [data.technologies] });
                    repaginate();
                    $scope.loading = false;
                });
            //.error(function (error) {
            //    $scope.errorMessage = "Some error ocurred";
            //    $scope.errorOcurred = true;
            //    $scope.loading = false;
            //});
        }

        var repaginate = function () {
            var totalPages = parseInt(parseInt($scope.count / $scope.pagination.pageSize) + ($scope.count % $scope.pagination.pageSize !== 0 ? 1 : 0));
            $scope.pages = Array.apply(null, { length: totalPages }).map(Number.call, Number);
        }

        var changePage = function (currentPage) {
            if (currentPage > 0 || currentPage <= $scope.pages.length) {
                $scope.pagination.currentPage = currentPage;
                search();
            }
        }

        var goToDetails = function (project) {
            $location.path("home/" + project.id + "/details");
        }

        var addNewProject = function () {
            $location.path("home/add");
        }

        var filtersApplied = function (evt, filterResponse) {
            var technologies = filterResponse.selectedFilters["Technologies"];
            _.each(_.keys(technologies), function (tech) {
                if (technologies[tech]) {
                    if (!(_.contains($scope.selectedFacets.technologies, tech)))
                        $scope.selectedFacets.technologies.push(tech);
                }
                else {
                    if (_.contains($scope.selectedFacets.technologies, tech)) {
                        var idx = _.indexOf($scope.selectedFacets.technologies, tech);
                        if (idx > -1)
                            $scope.selectedFacets.technologies.splice(idx, 1);
                    }
                }

            })
            search(true, filterResponse.selectedFacet);
        }

        var init = function () {
            $rootScope.breadcrumbs = $rootScope.breadcrumbs || [{}];
            $rootScope.breadcrumbs.pop();
            $rootScope.breadcrumbs.push({
                title: "Catalog",
                href: "/common/catalog"
            });

            $scope.pagination = {
                currentPage: 1,
                pageSize: 8
            };

            $scope.facets = {
                technologies: ["ASP.NET Core 1.0"]
            }

            $scope.selectedFacets = {
                technologies: []
            }

            $scope.search = search;
            $scope.goToDetails = goToDetails;
            $scope.addNewProject = addNewProject;
            $scope.changePage = changePage;

            $rootScope.$on("filter.applied", filtersApplied);

            search(true);
        }
        init();
    }

    module.controller("catalogController", ["$scope", "$rootScope", "$location", "catalogData", catalogController]);

}(angular.module("catalog")))