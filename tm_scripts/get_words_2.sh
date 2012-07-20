#!/bin/bash 

script_dir=$(cd $(dirname $0); echo ${PWD%/*})
cd $script_dir/www
x=238
while [ $x -lt 300 ]
do
  nice -n 19 php index.php get_words run $x
  x=$(( $x + 1 ))
done
