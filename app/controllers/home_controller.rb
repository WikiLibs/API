class HomeController < ApplicationController
  def index
    # p $tab
    # p $current_lib
    # p $current_function

    file_path = $tab[$current_lib][$current_function]["link"]
    @tmp = File.read(file_path)
  end
end
