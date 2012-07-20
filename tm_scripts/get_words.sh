#!/bin/bash 

script_dir=$(cd $(dirname $0); echo ${PWD%/*})
cd $script_dir/www
x=0
while [ $x -lt 48 ]
do
  nice -n 19 php index.php test_a run $x
  x=$(( $x + 1 ))
done
