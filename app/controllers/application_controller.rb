class ApplicationController < ActionController::Base
    protect_from_forgery  with: :exception
    skip_before_filter :verify_authenticity_token, if: -> { controller_name == 'sessions' && action_name == 'create' }
end
