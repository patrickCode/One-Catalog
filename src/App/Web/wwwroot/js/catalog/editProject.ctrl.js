(function (module) {

    var editProjectController = function ($scope, $rootScope, $location, $stateParams, adalAuthenticationService, catalogData, technologyData) {

        var getProject = function (projectId) {
            $scope.loading = true;
            catalogData.getProjectDetails(projectId)
                .then(function (data) {
                    $scope.project = data;
                    $scope.project.lastModifiedBy = { "alias": adalAuthenticationService.userInfo.userName, "name": "" };
                    _.each($scope.project.technologies, function (technology) {
                        $scope.techTags.push({"text": technology.name});
                    });
                    _.each($scope.project.contacts, function (contact) {
                        $scope.contactTags.push({"text": contact.alias});
                    });
                    $scope.loading = false;
                })  
        }
        
        var editProject = function () {
            $scope.submitting = true;

            $scope.project.technologies = [];
            $scope.project.contacts = [];

            $scope.techTags.forEach(function (tag) {
                var existingTech = _.findWhere($scope.project.technologies, { name: tag.text });
                if (existingTech === undefined || existingTech === null)
                    $scope.project.technologies.push({ "id": 0, "name": tag.text.replace(/-/g, ' '), "relatedTechologies": null });
            });

            $scope.contactTags.forEach(function (contact) {
                var existingContact = _.findWhere($scope.project.contacts, { alias: contact.text });
                if (existingContact === undefined || existingContact === null)
                    $scope.project.contacts.push({ "alias": contact.text, "name": "" });
            });

            $scope.errorOcurred = false;
            catalogData.editProject($scope.project)
                .then(function (data) {
                    $scope.submitting = false;
                    $location.path("home/" + $scope.project.id + "/details");
                }, function (error) {
                    $scope.submitting = false;
                    alert("Some unexpected error ocurred");
                });
        }

        $scope.getTechnologySuggestions = function (query) {
            return technologyData.getTechnologySuggestions(query);
        }

        var init = function() {
            $scope.submit = editProject;
            $scope.editMode = true;

            $scope.techTags = [];
            $scope.contactTags = [];
            $scope.submitting = false;

            $scope.project = {
                "id": 0,
                "name": "",
                "description": "",
                "abstract": "",
                "additionalDetails": "",
                "technologies": [],
                "createdBy": {  },
                "contacts": [],
                "codeLink": { "linkType": "Code", "href": null, "desciption": null },
                "previewLink": { "linkType": "Preview", "href": null, "desciption": null },
                "additionalLinks": [],
                "createdOn": null,
                "lastModifiedOn": null,
                "lastModifiedBy": { "alias": adalAuthenticationService.userInfo.userName, "name": "" },
                "isDeleted": false
            }
            var projectId = $stateParams.id;
            if (projectId !== undefined && projectId !== null && projectId > 0)
                getProject(projectId);
        }
        init();
    }

    module.controller("editProjectController", ["$scope", "$rootScope", "$location", "$stateParams", "adalAuthenticationService", "catalogData", "technologyData", editProjectController]);

}(angular.module("catalog")))