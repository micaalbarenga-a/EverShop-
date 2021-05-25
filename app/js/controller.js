'use strict'
app.factory('utils', function ($http, CONFIG) {
    let urlBase = CONFIG.ct_URL;
    return {
        login: (u) => $http.post(urlBase + '/login/login', u),
        logout: () => $http.post(urlBase + '/login/logout')
    }

})
    .controller("MainCtrl", function ($scope, utils, toaster, $state, $rootScope) {
        var m = $scope.m = {};



        m.login = () => {
            m.isLoading = true;
            utils.login(m.cus).then(
                ({ data }) => {
                    m.adm = data;
                    m.isLoading = false;
                    $rootScope.log = true;
                    $state.go('index');
                },

                ({ data }) => {
                    m.isLoading = false;
                    toaster.warning(data.Message);
                }


            );
        }

        m.logout = () => {
            m.isLoading = true;
            utils.logout().then(
                ({ data }) => {
                    m.isLoading = false;
                    $state.go('login');
                    $rootScope.log = false;
                },

                ({ data }) => {
                    m.isLoading = false;
                    $rootScope.log = false;
                    $state.go("login");
                }


            );
        }

    })

    .controller("IxCtrl", function ($state, $rootScope) {
        if (!$rootScope.log)
            $state.go("login");
    })

    ;
