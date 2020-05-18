using System;
using System.Collections.Generic;
using System.Text;

namespace WikiLibs.Shared
{
    public static class Permissions
    {
        #region SYMBOLS
        public const string CREATE_SYMBOL = "symbol.create";
        public const string UPDATE_SYMBOL = "symbol.update";
        public const string DELETE_SYMBOL = "symbol.delete";
        public const string OPTIMIZE_SYMBOL = "symbol.optimize";
        #endregion

        #region EXAMPLES
        /// <summary>
        /// Create an example for a given symbol
        /// </summary>
        public const string CREATE_EXAMPLE = "example.create";

        /// <summary>
        /// Patch an existing example
        /// </summary>
        public const string UPDATE_EXAMPLE = "example.update";

        /// <summary>
        /// Delete an example
        /// </summary>
        public const string DELETE_EXAMPLE = "example.delete";

        /// <summary>
        /// Create an example by requesting admin review
        /// </summary>
        public const string CREATE_EXAMPLE_REQUEST_POST = "example.requests.create.post";

        /// <summary>
        /// Patch an example by requesting admin review
        /// </summary>
        public const string CREATE_EXAMPLE_REQUEST_PATCH = "example.requests.create.patch";

        /// <summary>
        /// Delete an example by requesting admin review
        /// </summary>
        public const string CREATE_EXAMPLE_REQUEST_DELETE = "example.requests.create.delete";

        /// <summary>
        /// Patch your own example request
        /// </summary>
        public const string UPDATE_EXAMPLE_REQUEST = "example.requests.update";

        /// <summary>
        /// Delete your own example request
        /// </summary>
        public const string DELETE_EXAMPLE_REQUEST = "example.requests.delete";

        /// <summary>
        /// Apply an example request
        /// </summary>
        public const string APPLY_EXAMPLE_REQUEST = "example.requests.apply";

        /// <summary>
        /// List example requests
        /// </summary>
        public const string LIST_EXAMPLE_REQUEST = "example.requests.list";
        #endregion

        #region USERS
        public const string DELETE_USER = "user.delete";
        public const string DELETE_ME = "user.me.delete";
        public const string UPDATE_USER = "user.update";
        public const string UPDATE_ME = "user.me.update";
        #endregion

        #region BOTS
        public const string CREATE_BOT = "bot.create";
        public const string UPDATE_BOT = "bot.update";
        public const string DELETE_BOT = "bot.delete";
        public const string LIST_BOT = "bot.list";
        #endregion

        #region ADMIN_APIKEYS
        public const string CREATE_APIKEY = "apikey.create";
        public const string UPDATE_APIKEY = "apikey.update";
        public const string DELETE_APIKEY = "apikey.delete";
        public const string LIST_APIKEY = "apikey.list";
        #endregion

        #region ADMIN_GROUPS
        public const string CREATE_GROUP = "group.create";
        public const string UPDATE_GROUP = "group.update";
        public const string DELETE_GROUP = "group.delete";
        public const string LIST_GROUP = "group.list";
        #endregion

        #region ADMIN_SYMBOL_LANGS
        public const string CREATE_SYMBOL_LANG = "symbol.lang.create";
        public const string UPDATE_SYMBOL_LANG = "symbol.lang.update";
        public const string DELETE_SYMBOL_LANG = "symbol.lang.delete";
        #endregion

        #region ADMIN_SYMBOL_TYPES
        public const string CREATE_SYMBOL_TYPE = "symbol.type.create";
        public const string UPDATE_SYMBOL_TYPE = "symbol.type.update";
        public const string DELETE_SYMBOL_TYPE = "symbol.type.delete";
        #endregion
    }
}
