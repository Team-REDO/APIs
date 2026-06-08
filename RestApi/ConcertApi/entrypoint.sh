#!/bin/bash
# ConcertApi Entrypoint Script
# Ensures database connectivity before starting the application

set -e

echo "ConcertApi: Starting..."

# Wait for MySQL to be ready
echo "ConcertApi: Waiting for MySQL to be ready..."
until nc -z -v -w30 mysql 3306 2>&1; do
  echo 'ConcertApi: Waiting for MySQL...'
  sleep 1
done

echo "ConcertApi: MySQL is ready!"
echo "ConcertApi: Starting application..."

# Run the application
exec dotnet ConcertApi.dll
