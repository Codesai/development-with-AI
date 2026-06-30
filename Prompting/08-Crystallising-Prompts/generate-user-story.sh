#!/bin/bash

echo "=== User Story Generator ==="
echo ""

read -p "Product name: " PRODUCT_NAME
read -p "Main users: " USER_TYPES
read -p "Business goal: " BUSINESS_GOAL
read -p "Feature name: " FEATURE_NAME
read -p "Primary user: " PRIMARY_USER
read -p "User goal: " USER_GOAL
read -p "Reason this feature exists: " FEATURE_REASON
read -p "Scope: " SCOPE
read -p "Included scenarios: " INCLUDED_SCENARIOS
read -p "Excluded scenarios: " EXCLUDED_SCENARIOS

echo ""
echo "=== Generated User Story ==="
echo ""
echo "# $FEATURE_NAME"
echo ""
echo "## User Story"
echo "As a $PRIMARY_USER, I want to $USER_GOAL, so that $FEATURE_REASON."
echo ""
echo "## Acceptance Criteria"
echo "**Scenario: $PRIMARY_USER completes $FEATURE_NAME**"
echo "Given the $PRIMARY_USER has access to $FEATURE_NAME"
echo "When they $USER_GOAL"
echo "Then $SCOPE works as expected"
echo "And $INCLUDED_SCENARIOS are handled correctly"
echo "And $EXCLUDED_SCENARIOS are out of scope"
echo ""
echo "## Definition of Done"
echo "- Acceptance criteria scenarios pass"
echo "- Code reviewed by at least one peer"
echo "- Deployed to production"
