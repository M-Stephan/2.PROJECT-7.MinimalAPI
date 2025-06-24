#!/bin/bash

# wait-for.sh

host="$1"
shift
cmd="$@"

until nc -z "$host" 1433; do
  echo "Waiting for SQL Server at $host:1433..."
  sleep 3
done

echo "SQL Server is up - executing command"
exec $cmd
