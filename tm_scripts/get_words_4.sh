#!/bin/bash 

script_dir=$(cd $(dirname $0); echo ${PWD%/*})
cd $script_dir/www
x=400
while [ $x -lt 500 ]
do
  nice -n 19 php index.php get_words run $x
  x=$(( $x + 1 ))
done
