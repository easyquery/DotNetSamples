; (function ($, window) {

    //Ensure that global variables exist
    var EQ = window.EQ = window.EQ || {};

    EQ.listcache = EQ.listcache || {};

    EQ.listcache.localstorage = {
       
        _listCache: {

            _listPrefix: "eq-list-",

            get: function (key) {
                var listJSON = localStorage.getItem(this._listPrefix + key);
                return listJSON ? JSON.parse(listJSON) : null;
            },

            set: function (key, value) {
                localStorage.setItem(this._listPrefix + key, JSON.stringify(value));
            },

            clear: function () {
                var removeItems = [];
                for (var i = 0; i < localStorage.length; i++) {
                    var key = localStorage.key(i);
                    if (key.startsWith(this._listPrefix)) {
                        removeItems.push(key);
                    }
                }

                for (var i = 0; i < removeItems.length; i++) {
                    localStorage.removeItem(removeItems[i]);
                }
            }
        },

        getCacheObject: function () {
            return this._listCache;
        },

        clearCache: function () {
            return this._listCache.clear();
        }


    };

})(jQuery, window);