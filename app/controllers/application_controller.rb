require 'json'

class ApplicationController < ActionController::Base
    protect_from_forgery  with: :exception
    skip_before_action :verify_authenticity_token

    file_path = "file.json"

    tmp = File.read(file_path)

    $tab = JSON.parse(tmp)

    $current_lib = "lib"
    $current_function = "fun"
    # $tab.keys.each do |lib|
    #     p lib
    #     $tab[lib].keys.each do |function|
    #         p $tab[lib][function]["name"]
    #     end
    # end
end
