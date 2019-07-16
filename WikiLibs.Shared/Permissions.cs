﻿using System;
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
        /// Accept an example request
        /// </summary>
        public const string VALIDATE_EXAMPLE_REQUEST = "example.requests.validate";

        /// <summary>
        /// Decline an example request
        /// </summary>
        public const string INVALIDATE_EXAMPLE_REQUEST = "example.requests.invalidate";

        /// <summary>
        /// List example requests
        /// </summary>
        public const string LIST_EXAMPLE_REQUEST = "example.requests.list";
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
    }
}
