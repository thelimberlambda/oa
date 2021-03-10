using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace oa
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }

        static void RunIt()
        {
            // Day offset: how many days *previous* to today that we want to be doign this
            // for (useful, if for instance, you want to see your time for *yesterday*, and
            // not today).  For now, just change the below value and re-run.
            //
            var dayOffset = 0;
            var date_time_match_regex = @"(?P<day>\d{2})-(?P<month>\d{2})-(?P<year>\d{2}) " +
                                        @"(?P<hour>\d{2}):(?P<minute>\d{2}):(?P<second>\d{2})";

            var start_match = new Regex("^" + date_time_match_regex + ".*");
            var end_match = new Regex("} " + date_time_match_regex + ".*");

            var entry_dict = new Dictionary<string, string>();

        }

        static Tuple<string, string, string, string, string, string> parse_timestamp_data(match_ob)
        {
            return (int(match_ob.group('year')),
                    int(match_ob.group('month')),
                    int(match_ob.group('day')),
                    int(match_ob.group('hour')),
                    int(match_ob.group('minute')),
                    int(match_ob.group('second')))
        }


    }





def diff_mins(start_timestamp, end_timestamp):
    return ((end_timestamp[3] - start_timestamp[3])*60 +
            (end_timestamp[4] - start_timestamp[4]))


def accumulate_entry(start_timestamp, end_timestamp, project, entry_content):
    sts = (start_timestamp[0], start_timestamp[1], start_timestamp[2])
    if sts != today_t:
        return
    if project not in entry_dict:
        entry_dict[project] = {'mins': 0, 'content': []}
    entry_dict[project]['mins'] += diff_mins(start_timestamp, end_timestamp)
    entry_dict[project]['content'].extend(entry_content)


def hours_and_mins(mins_only):
    return (mins_only//60, mins_only%60)


def rounded_hours_and_mins(h, m):
    if not h and m < 15:
        rounded_mins = 15
    else:
        rounded_mins = round(m/15)*15
    return (h+rounded_mins//60, rounded_mins%60)


def formatted_duration(h, m):
    return "{}:{:02}".format(h, m)



today = datetime.now() + timedelta(days=-1*DAY_OFFSET)
today_t = (today.year%100, today.month, today.day)
state = 'look_for_entry'


for l in sys.stdin:
    stripped = l.strip()
    if state == 'look_for_entry':
        match = start_match.match(stripped)
        if match:
            state = 'started_entry'
            start_timestamp = parse_timestamp_data(match)
            continue
    elif state == 'started_entry':
        if not stripped:
            continue
        project = stripped
        state = 'got_project'
        entry_content = []
    elif state == 'got_project':
        if not stripped:
            continue
        match = end_match.match(stripped)
        if match:
            state = 'look_for_entry'
            end_timestamp = parse_timestamp_data(match)
            accumulate_entry(
                start_timestamp,
                end_timestamp,
                project,
                entry_content)
        else:
            entry_content.append(re.sub(r"^-\s*", "", stripped))

total = 0
for k in entry_dict.keys():
    print("\nEntry: \"{}\"".format(k))
    (h, m) = hours_and_mins(entry_dict[k]['mins'])
    (rh, rm) = rounded_hours_and_mins(h, m)
    print("Minutes: {} ({}) ({})".format(
        formatted_duration(rh, rm),
        formatted_duration(h, m),
        entry_dict[k]['mins']))
    total += rh*60+rm
    if entry_dict[k]['content']:
        print("Content:\n- {}".format('\n- '.join(entry_dict[k]['content'])))

print("\nTOTAL: {} ({})".format(formatted_duration(*hours_and_mins(total)), total))

}
