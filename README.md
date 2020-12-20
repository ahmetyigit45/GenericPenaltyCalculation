
# Generic Penalty Calculation

### Description
```
1)	The library function should make usage of:
    a.	Date start, -- this is the date penalty calculation should start. Inclusive.
    b.	Date end, -- this is the date penalty calculation should finish. Inclusive.
    c.	Country of operation.
2)	The library function should return
    a.	Calculated penalty
3)	Penalty should be calculated for BUSINESS days only. That means your calculation should take
    account of weekends and national holidays. 
4)	You should develop your own algorithm for business day count. 
    a.	Hint: Try to use DayOfWeek enumeration of .Net
5)	Each late business day will be penalized for x amount of money. This amount should be configurable
    based on country of operation.
    (DailyPenaltyFee parameter in App.config)
6)	Penalty will not apply for an allowed amount of days (PenaltyAppliesAfter parameter in App.config)


```
