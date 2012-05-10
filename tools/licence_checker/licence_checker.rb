#!/usr/bin/ruby
current_directory = File.dirname(__FILE__)

@@licences = Dir.glob(File.join(current_directory, "formats/*.txt")).map do |file|
   File.read(file);
end

def needs_licence text
  puts @@licences
  puts text
  return @@licences.select {|x| text[x]}.empty?
end

Dir.glob(File.join(current_directory, '../../src/**/*.cs'))[0..1].each do|f|
  puts f if needs_licence File.read(f)
end
